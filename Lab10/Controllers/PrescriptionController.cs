using System.IdentityModel.Tokens.Jwt;
using System.Text;
using APBD9.DTOs;
using APBD9.Helpers;
using APBD9.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace APBD9.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PrescriptionController : ControllerBase
{
    private readonly PrescriptionDbContext _context;
    private readonly IConfiguration _configuration;

    public PrescriptionController(PrescriptionDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult> PostPrescription(PrescriptionDto prescriptionDto)
    {
            if (prescriptionDto.DueDate < prescriptionDto.Date)
            {
                return BadRequest("DueDate must be >= Date");
            }
            
            if (prescriptionDto.Medicaments.Count > 10)
            {
                return BadRequest("Max 10 Medicaments");
            }
            
            var patient = await _context.Patients
                .FirstOrDefaultAsync(p => p.IdPatient == prescriptionDto.PatientId);

            if (patient == null)
            {
                patient = new Patient
                {
                    IdPatient = prescriptionDto.PatientId,
                    FirstName = prescriptionDto.PatientFirstName,
                    LastName = prescriptionDto.PatientLastName,
                    BirthDate = prescriptionDto.PatientBirthdate
                };
                _context.Patients.Add(patient);
            }
            
            var medicamentIds = prescriptionDto.Medicaments.Select(m => m.MedicamentId).ToList();
            var existingMedicaments = await _context.Medicaments
                .Where(m => medicamentIds.Contains(m.IdMedicament))
                .ToListAsync();

            if (existingMedicaments.Count != prescriptionDto.Medicaments.Count)
            {
                return BadRequest("Medicament does not exist");
            }
            
            var prescription = new Prescription
            {
                Date = prescriptionDto.Date,
                DueDate = prescriptionDto.DueDate,
                Patient = patient,
                Doctor = await _context.Doctors.FindAsync(prescriptionDto.DoctorId),
                PrescriptionMedicaments = prescriptionDto.Medicaments.Select(m => new PrescriptionMedicament
                {
                    IdMedicament = m.MedicamentId,
                    Dose = m.Dose,
                    Details = m.Description
                }).ToList()
            };

            _context.Prescriptions.Add(prescription);
            await _context.SaveChangesAsync();

            return Ok("Created successfully");
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<Patient>> GetPatient(int id)
    {
        var patient = await _context.Patients
            .Include(p => p.Prescriptions)
            .ThenInclude(p => p.PrescriptionMedicaments)
            .ThenInclude(pm => pm.Medicament)
            .Include(p => p.Prescriptions)
            .ThenInclude(p => p.Doctor)
            .FirstOrDefaultAsync(p => p.IdPatient == id);

        if (patient == null)
        {
            return NotFound("Patient not found.");
        }

        var sortedPrescriptions = patient.Prescriptions
            .OrderBy(p => p.DueDate)
            .ToList();

        var result = new
        {
            IdPatient = patient.IdPatient,
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            BirthDate = patient.BirthDate,
            Prescriptions = sortedPrescriptions.Select(p => new
            {
                IdPrescription = p.IdPrescription,
                Date = p.Date,
                DueDate = p.DueDate,
                Doctor = new
                {
                    p.Doctor.IdDoctor,
                    p.Doctor.FirstName,
                    p.Doctor.LastName,
                    p.Doctor.Email
                },
                Medicaments = p.PrescriptionMedicaments.Select(pm => new
                {
                    pm.Medicament.IdMedicament,
                    pm.Medicament.Name,
                    pm.Medicament.Description,
                    pm.Medicament.Type,
                    pm.Dose,
                    pm.Details
                }).ToList()
            }).ToList()
        };

        return Ok(result);
    }
    
    [AllowAnonymous]
    [HttpPost("register")]
    public IActionResult RegisterStudent(LoginRegisterRequest registerRequest)
    {
        var hashedPasswordAndSalt = SecurityHelper.GetHashedPasswordAndSalt(registerRequest.Password);

        var user = new AppUser
        {
            Username = registerRequest.Login,
            Password = hashedPasswordAndSalt.Item1,
            Salt = hashedPasswordAndSalt.Item2,
            RefreshToken = SecurityHelper.GenerateRefreshToken(),
            RefreshTokenExp = DateTime.Now.AddDays(1)
        };

        _context.AppUsers.Add(user);
        _context.SaveChanges();

        return Ok();
    }
    
    [AllowAnonymous]
    [HttpPost("login")]
    public IActionResult Login(LoginRegisterRequest loginRequest)
    {
        AppUser user = _context.AppUsers.Where(u => u.Username == loginRequest.Login).FirstOrDefault();

        string passwordHashFromDb = user.Password;
        string curHashedPassword = SecurityHelper.GetHashedPasswordWithSalt(loginRequest.Password, user.Salt);

        if (passwordHashFromDb != curHashedPassword)
        {
            return Unauthorized();
        }

        SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));

        SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        JwtSecurityToken token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            expires: DateTime.Now.AddMinutes(10),
            signingCredentials: creds
        );

        user.RefreshToken = SecurityHelper.GenerateRefreshToken();
        user.RefreshTokenExp = DateTime.Now.AddDays(1);
        _context.SaveChanges();

        return Ok(new
        {
            accessToken = new JwtSecurityTokenHandler().WriteToken(token),
            refreshToken = user.RefreshToken
        });
    }
    
    [Authorize(AuthenticationSchemes = "IgnoreTokenExpirationScheme")]
    [HttpPost("refresh")]
    public IActionResult Refresh(RefreshTokenRequest refreshToken)
    {
        AppUser user = _context.AppUsers.Where(u => u.RefreshToken == refreshToken.RefreshToken).FirstOrDefault();
        if (user == null)
        {
            throw new SecurityTokenException("Invalid refresh token");
        }

        if (user.RefreshTokenExp < DateTime.Now)
        {
            throw new SecurityTokenException("Refresh token expired");
        }

        SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));

        SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        JwtSecurityToken jwtToken = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            expires: DateTime.Now.AddMinutes(10),
            signingCredentials: creds
        );

        user.RefreshToken = SecurityHelper.GenerateRefreshToken();
        user.RefreshTokenExp = DateTime.Now.AddDays(1);
        _context.SaveChanges();

        return Ok(new
        {
            accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken),
            refreshToken = user.RefreshToken
        });
    }
}
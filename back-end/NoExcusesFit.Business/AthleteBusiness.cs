using NoExcusesFit.Domain.DTOs.Athlete;
using NoExcusesFit.Domain.Entities;
using NoExcusesFit.Domain.Enums;
using NoExcusesFit.Domain.Exceptions;
using NoExcusesFit.Domain.Interfaces;
using NoExcusesFit.Domain.Interfaces.Business;
using NoExcusesFit.Domain.Interfaces.Repositories;

namespace NoExcusesFit.Business
{
    public class AthleteBusiness : IAthleteBusiness
    {
        private readonly IAthleteRepository _athleteRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IUserAccountRepository _userAccountRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AthleteBusiness(
            IAthleteRepository athleteRepository, 
            IUserRoleRepository userRoleRepository, 
            IUserAccountRepository userAccountRepository,
            IUnitOfWork unitOfWork)
        {
            _athleteRepository = athleteRepository;
            _userRoleRepository = userRoleRepository;
            _userAccountRepository = userAccountRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<AthleteResponseDto>> GetAllAsync(int page, int pageSize)
        {
            var skip = (page - 1) * pageSize;

            var athletes = await _athleteRepository.GetAllAsync(skip, pageSize);

            return athletes.Select(a => new AthleteResponseDto(a.Id, a.UserAccountId, a.CoachId, a.FirstName, a.Email));
        }

        public async Task<AthleteResponseDto> GetByIdAsync(Guid id)
        {
            var athlete = await _athleteRepository.GetByIdAsync(id);
            if (athlete is null)
                throw new NotFoundException("Atleta não encontrado.");

            return new AthleteResponseDto(athlete.Id, athlete.UserAccountId, athlete.CoachId, athlete.FirstName, athlete.Email);
        }

        public async Task<AthleteResponseDto> RegisterAsync(RegisterAthleteRequestDto request)
        {
            var userExisting = await _userAccountRepository.GetByEmailAsync(request.Email);
            if (userExisting is not null)
                throw new InvalidOperationException("Email já cadastrado.");

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            var user = new UserAccount(request.FirstName, request.Email, passwordHash);
            var athlete = new Athlete(user.Id, request.CoachId);
            var athleteRole = new UserRole(user.Id, (int)UserRoleType.Athlete);

            _unitOfWork.Begin();

            await _userAccountRepository.CreateAsync(user, _unitOfWork.Connection, _unitOfWork.Transaction);
            await _athleteRepository.AddAsync(athlete, _unitOfWork.Connection, _unitOfWork.Transaction);
            await _userRoleRepository.AssignUserRoleAsync(athleteRole, _unitOfWork.Connection, _unitOfWork.Transaction);

            _unitOfWork.Commit();

            return new AthleteResponseDto(athlete.Id, athlete.UserAccountId, athlete.CoachId, user.FirstName, user.Email);
        }

        public async Task<CreateAthleteResponseDto> AddAsync(CreateAthleteRequestDto request)
        {
            var athlete = new Athlete(request.UserAccountId, request.CoachId);
            var athleteRole = new UserRole(athlete.UserAccountId, (int)UserRoleType.Athlete);

            _unitOfWork.Begin();

            await _athleteRepository.AddAsync(athlete, _unitOfWork.Connection, _unitOfWork.Transaction);
            await _userRoleRepository.AssignUserRoleAsync(athleteRole, _unitOfWork.Connection, _unitOfWork.Transaction);

            _unitOfWork.Commit();

            return new CreateAthleteResponseDto(athlete.Id, athlete.UserAccountId, athlete.CoachId);
        }

        public async Task UpdateAsync(Guid id, UpdateAthleteRequestDto request)
        {
            if (request.CoachId == Guid.Empty)
                throw new ArgumentException("CoachId inválido.");

            var athlete = await _athleteRepository.GetByIdAsync(id);
            if (athlete is null)
                throw new NotFoundException("Atleta não encontrado.");

            await _athleteRepository.UpdateAsync(id, request);
        }

        public async Task DeleteAsync(Guid id)
        {
            var athlete = await _athleteRepository.GetByIdAsync(id);

            if (athlete is null)
                throw new NotFoundException("Atleta não encontrado.");

            _unitOfWork.Begin();

            await _athleteRepository.DeleteAsync(id, _unitOfWork.Connection, _unitOfWork.Transaction);
            await _userRoleRepository.DeleteRoleAsync(
                athlete.UserAccountId, 
                (int)UserRoleType.Athlete, 
                _unitOfWork.Connection, 
                _unitOfWork.Transaction);

            _unitOfWork.Commit();
        }
    }
}

using NoExcusesFit.Domain.DTOs.Speciality;
using NoExcusesFit.Domain.Entities;
using NoExcusesFit.Domain.Exceptions;
using NoExcusesFit.Domain.Interfaces.Business;
using NoExcusesFit.Domain.Interfaces.Repositories;

namespace NoExcusesFit.Business;

public class SpecialityBusiness : ISpecialityBusiness
{
    private readonly ISpecialityRepository _specialityRepository;

    public SpecialityBusiness(ISpecialityRepository specialityRepository)
    {
        _specialityRepository = specialityRepository;
    }

    public async Task<IEnumerable<SpecialityDto>> GetAllAsync()
    {
        var speciality = await _specialityRepository.GetAllAsync();

        return speciality.Select(s => new SpecialityDto(s.Id, s.Description, s.CreatedAt));
    }

    public async Task<SpecialityResponseDto> AddAsync(CreateSpecialityRequestDto request)
    {
        var speciality = new Speciality(request.Description);
        var id = await _specialityRepository.AddAsync(speciality);

        return new SpecialityResponseDto(id);
    }

    public async Task UpdateAsync(int id, UpdateSpecialityRequestDto request)
    {
        
        var speciality = await _specialityRepository.GetByIdAsync(id);
        if (speciality is null)
            throw new NotFoundException("Especialidade não encontrada.");

        speciality.UpdateDescription(request.Description);

        await _specialityRepository.UpdateAsync(speciality);
    }

    public async Task DeleteAsync(int id)
    {
        var speciality = await _specialityRepository.GetByIdAsync(id);
        if (speciality is null)
            throw new NotFoundException("Especialidade não encontrada.");

        await _specialityRepository.DeleteAsync(id);
    }
}
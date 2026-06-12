using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WMS.Application.DTOs.Client;
using WMS.Application.Interfaces;

namespace WMS.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Admin")]
public class ClientController : ControllerBase
{
    private readonly IClientService _clientService;

    public ClientController(IClientService clientService)
        => _clientService = clientService;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _clientService.GetAllAsync();
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _clientService.GetByIdAsync(id);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateClientDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var id = await _clientService.CreateAsync(dto);
        return Ok(new { ClientId = id });
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateClientDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var result = await _clientService.UpdateAsync(dto);
        if (!result) return NotFound();
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _clientService.DeleteAsync(id);
        if (!result) return NotFound();
        return Ok();
    }
}

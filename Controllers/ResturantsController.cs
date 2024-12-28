using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Resturant.Application.Resturant;
using Resturant.Application.Resturant.Commonds.CreateResturant;
using Resturant.Application.Resturant.Commonds.DeleteResturant;
using Resturant.Application.Resturant.Commonds.UpdateResturnat;
using Resturant.Application.Resturant.Dtos;
using Resturant.Application.Resturant.Queries.GetAllResturants;
using Resturant.Application.Resturant.Queries.GetResturantById;

namespace Resturant.API.Controllers;

//[ApiController]
[Route("api/resturants")]
public class ResturantsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var resturant = await mediator.Send(new GetAllResturantsQuery());
        return Ok(resturant);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute]int id)
    {
        var resturant = await mediator.Send(new GetResturantByIdQuery(id));
        if (resturant == null)
        {
            return NotFound();
        }
        else
            return Ok(resturant);
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteById([FromRoute] int id)
    {
        var isDeleted = await mediator.Send(new DeleteResturantCommond(id));
        if (isDeleted)
        {
            return NoContent();
        }

        return NotFound();
    }
    [HttpPost]
    public async Task<IActionResult> CreateResturant(CreateResturantCommond commond)
    { 
        if(!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        int id = await mediator.Send(commond);
        return CreatedAtAction(nameof(GetById), new {id}, null);
    }
    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateResturant([FromRoute] int id, UpdateResturnatCommond commond)
    {
        commond.Id = id;
        var isUpdated = await mediator.Send(commond);
       
        if(isUpdated)
            return NoContent();

        return NotFound();
        
    }
}


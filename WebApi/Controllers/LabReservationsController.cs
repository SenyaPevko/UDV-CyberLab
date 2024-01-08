﻿using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using WebApi.Services;
using Microsoft.AspNetCore.Authorization;

namespace WebApi.Controllers
{
    [Route("/api/schedule")]
    [ApiController]
    public class LabReservationsController : ControllerBase
    {
        private readonly LabReservationsService _labReservationsService;
        private readonly UserService _userService;

        public LabReservationsController(LabReservationsService labReservationsService, UserService userService)
        {
            _labReservationsService = labReservationsService;
            _userService = userService;
        }

        [HttpPost("create")]
        [Authorize(Roles = "Admin, Teacher")]
        public async Task<ActionResult> Create(CreateLabReservationRequest creationRequest)
        {
            try
            {
                var labReservation = new LabReservation()
                {
                    Description = creationRequest.Description,
                    Theme = creationRequest.Theme,
                    Lab = creationRequest.Lab,
                    Reservor = await _userService.GetAsyncById(creationRequest.ReservorId),
                    TimeEnd = new DateTime(creationRequest.TimeEnd),
                    TimeStart = new DateTime(creationRequest.TimeStart),
                };
                await _labReservationsService.CreateAsync(labReservation);
                return StatusCode(201);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("get{id}")]
        public async Task<ActionResult<LabReservation>> Get(string id)
        {
            try
            {
                return await _labReservationsService.GetByIdAsync(id);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("get")]
        [Authorize(Roles = "Admin")]
        public async Task<List<LabReservation>> GetAll()
        {
            return await _labReservationsService.GetAllAsync();
        }

        [HttpPost("update")]
        [Authorize(Roles = "Admin, Teacher")]
        public async Task<IActionResult> Update(CreateLabReservationRequest creationRequest, string userId)
        {
            try
            {
                var labReservation = new LabReservation()
                {
                    Description = creationRequest.Description,
                    Theme = creationRequest.Theme,
                    Lab = creationRequest.Lab,
                    Reservor = await _userService.GetAsyncById(creationRequest.ReservorId),
                    TimeEnd = new DateTime(creationRequest.TimeEnd),
                    TimeStart = new DateTime(creationRequest.TimeStart),
                };
                await _labReservationsService.UpdateAsync(labReservation, userId);
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpDelete("delete")]
        [Authorize(Roles = "Admin, Teacher")]
        public async Task<IActionResult> Delete(string labReservationId, string userId)
        {
            try
            {
                await _labReservationsService.DeleteAsync(labReservationId, userId);
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using SAPPub.Core.Interfaces.Services.KS4.Attendance;
using SAPPub.Web.Areas.Profiles.ViewModels.Attendance;
using SAPPub.Web.Constants;

namespace SAPPub.Web.Areas.Profiles.Controllers;

[Area("Profiles")]
public class AttendanceController : Controller
{
    [HttpGet]
    [Route("school/{urn}/{schoolName}/attendance", Name = RouteConstants.SecondaryAttendance)]
    public async Task<IActionResult> Attendance(
    [FromServices] IAttendanceService attendanceService,
    string urn,
    string schoolName,
    CancellationToken ct)
    {
        var attendanceDetails = await attendanceService.GetAttendenceDetailsAsync(urn, ct);
        var model = AttendanceViewModel.Map(attendanceDetails);
        return View(model);
    }

}

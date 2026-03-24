using AngularDemoAPI.Helpers;
using AngularDemoAPI.Models.ViewModels.Meeting;
using AngularDemoAPI.Services.CurrentUser;
using AngularDemoAPI.Services.Meetings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AngularDemoAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class MeetingController : ControllerBase
    {
        private readonly IMeetingService _meetingService;
        private readonly ICurrentUserService _currentUser;

        public MeetingController(IMeetingService meetingService, ICurrentUserService currentUser)
        {
            _meetingService = meetingService;
            _currentUser = currentUser;
        }

        [HttpPost]
        public async Task<IActionResult> CreateMeeting(MeetingRequestModel request)
        {
            var result = await _meetingService.CreateAsync(request, _currentUser.Role, _currentUser.UserId);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetTeacherMeetings()
        {
            var meetings = await _meetingService.GetByTeacherAsync(_currentUser.UserId ?? 0);
            return Ok(meetings);
        }

        [HttpGet]
        public async Task<IActionResult> GetSchoolMeetings(int schoolId)
        {
            var meetings = await _meetingService.GetBySchoolAsync(schoolId);
            return Ok(meetings);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMeetings(string role, int? schoolId)
        {
            var meetings = await _meetingService.GetAllMeetingsAsync(role, schoolId);
            return Ok(meetings);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMeetingById(int id)
        {
            var meeting = await _meetingService.GetByIdAsync(id);
            if (meeting == null) return NotFound();
            return Ok(meeting);
        }

        // ─── Notifications ────────────────────────────────────────────────────

        /// <summary>
        /// Returns all meetings relevant to the logged-in user along with their notification/attendance state.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetMyNotifications()
        {
            var notifications = await _meetingService.GetMyNotificationsAsync();
            return Ok(notifications);
        }

        /// <summary>
        /// Marks a notification as seen for the logged-in user (creates or updates the attendance row).
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> AcknowledgeNotification(int meetingId)
        {
            await _meetingService.AcknowledgeNotificationAsync(meetingId);
            return Ok(new { message = "Notification acknowledged." });
        }

        // ─── Attendance ───────────────────────────────────────────────────────

        /// <summary>
        /// Called when the user joins the Jitsi room. Marks IsPresent = true and records JoinTime.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> JoinMeeting(int meetingId)
        {
            await _meetingService.JoinMeetingAsync(meetingId);
            return Ok(new { message = "Joined meeting." });
        }

        /// <summary>
        /// Called when the user leaves the Jitsi room. Records LeaveTime.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> LeaveMeeting(int meetingId)
        {
            await _meetingService.LeaveMeetingAsync(meetingId);
            return Ok(new { message = "Left meeting." });
        }

        /// <summary>
        /// Returns full attendance report for a meeting. Accessible by Teacher, Principal, Admin roles.
        /// </summary>
        [HttpGet]
        [Authorize(Roles = $"{nameof(UserRole.Teacher)},{nameof(UserRole.Principal)},{nameof(UserRole.SchoolAdmin)},{nameof(UserRole.SuperAdmin)}")]
        public async Task<IActionResult> GetAttendance(int meetingId)
        {
            var attendance = await _meetingService.GetAttendanceAsync(meetingId);
            return Ok(attendance);
        }
    }
}

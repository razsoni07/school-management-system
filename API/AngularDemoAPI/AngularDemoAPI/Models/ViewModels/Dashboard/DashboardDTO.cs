namespace AngularDemoAPI.Models.ViewModels.Dashboard
{
    public class DashboardDTO
    {
        public class AdminDashboardDto
        {
            public int TotalUsers { get; set; }
            public int TotalPrincipals { get; set; }
            public int TotalTeachers { get; set; }
            public int TotalContacts { get; set; }
        }

        public class PrincipalDashboardDto
        {
            public int TotalTeachers { get; set; }
            public int TotalContacts { get; set; }
        }
    }
}

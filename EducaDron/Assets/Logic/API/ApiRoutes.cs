using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Logic.API
{
    public static class ApiRoutes
    {
        public static class Users
        {
            public const string Login = "/api/users/login";
            public const string Register = "/api/users/register";
            public const string Points = "/api/users/points/";
            public const string PointsRanking = "/api/users/points/ranking";
            public const string UpdatePoints = "/api/users/points/update-points";
        }

        public static class Progress
        {
            public const string Base = "/api/progress/";
        }
    }
}

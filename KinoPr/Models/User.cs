using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinoPr
{
    public class User
    {
        public int id { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public string patronymic { get; set; }
        public string phone_number { get; set; }
        public DateTime birth { get; set; }
        public string login { get; set; }
        public string email { get; set; }
        public string api_token { get; set; }
        public int role_id { get; set; }

        // Не включаем пароль в вывод
        public string password { get; set; }
        public class UserResponse
        {
            public List<User> Data { get; set; }
        }
    }
}

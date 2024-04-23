using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinoPr
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime Birth { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public string ApiToken { get; set; }
        public int RoleId { get; set; }

        // Не включаем пароль в вывод
        public string Password { get; set; }
    }
}

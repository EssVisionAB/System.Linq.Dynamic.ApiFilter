using System;
using System.Collections.Generic;
using System.Text;

namespace xApiFilterTest.Db
{
    public class Model
    {
        public Model()
        {
            Contacts = new HashSet<Contact>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime? RegisteredDate { get; set; }

        public int? ResponsibleId { get; set; }

        public User Responsible { get; set; }

        public ICollection<Contact> Contacts { get; set; }

        public string Extension { get; set; }


    }

    public class User
    {
        public User()
        {
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }
    }

    public class Contact
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ContactPerson { get; set; }

        public string Address { get; set; }

        public string CoAddress { get; set; }

        public string ZipCode { get; set; }

        public string Region { get; set; }

        public string Country { get; set; }

        public int ModelId { get; set; }

        public Model Model { get; set; }
    }
}

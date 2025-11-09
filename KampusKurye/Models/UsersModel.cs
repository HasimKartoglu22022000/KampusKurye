using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KampusKurye.Models
{
    public class UsersModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int user_id { get; set; }

        public Guid user_guid { get; set; }
        public string user_name { get; set; }
        public string user_password { get; set; }
        public string user_firstname { get; set; }
        public string user_lastname { get; set; }
        public string user_email { get; set; }
        public string user_phone { get; set; }
        public string user_address { get; set; }
        public int user_role { get; set; }
        public string? user_img_url { get; set; }
    }
}

namespace Com.Scm.Email
{
    public class EmailAddress
    {
        public string Name { get; set; }
        public string Address { get; set; }

        public void Trim()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                Name = Address;
            }
        }
    }
}

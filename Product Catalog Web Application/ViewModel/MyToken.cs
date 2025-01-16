namespace Product_Catalog_Web_Application.ViewModel
{
    public class MyToken
    {
        public string Token { get; set; }
        public string ExpireDate { get; set; }
        public MyToken(string _Token, string _ExpireDate)
        {
            this.Token = _Token;
            this.ExpireDate = _ExpireDate;
        }
    }
}

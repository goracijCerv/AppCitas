namespace Apcitas.WebService.Entities;

public class AppUser
{
    //la forma de c# se puede resumir en esta linea
    public int Id { get; set; }
    public string UserName { get; set; }
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string KnowAs { get; set; }
    public DateTime Created { get; set; } = DateTime.Now;
    public DateTime LastActive { get; set; } = DateTime.Now;
    public string Gender { get; set; }
    public string Introduction { get; set; }
    public string Lokingfor { get; set; }
    public string Interests { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    public ICollection<Photo> Photos { get; set; }
    public ICollection<UserLike> LikedByUsers { get; set; }
    public ICollection<UserLike> LikedUsers { get; set; }
    public ICollection<Message> MessagesSent { get; set; }
    public ICollection<Message> MessagesReceived { get; set; }
    //metodos

    //public int GetAge()
    //{
    //  return DateOfBirth.CalculeteAge();
    //}

    //ESta es la froma en  la que se realiza en c#
    /*private int myVar;
    public int MyVar
    {
        get { return myVar; }
        set { myVar = value; }
    }*/
    //equivalente a lo anterior realizado pero en focado a lo que conocemos comun mente
    /*
     * public int getMyVar(){
     *    return MyVar;
     * }
     * public void setMyVar(int valor){
     *    MyVar=valor;
     * }
      */
}

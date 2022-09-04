namespace Apcitas.WebService.Entities;

public class AppUser
{
    //la forma de c# se puede resumir en esta linea
    public int Id { get; set; }
    public string UserName { get; set; }
    

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

// JScript File

function getIDData() {
    var strTemp;
    var strTemp2;
    var strTemp3;
    // schrijf een Script om de data in controls te plaatsen
    // Volgende methodes kunt u gebruiken
    strTemp = document.BEIDApplet.getName(); //als return waarde : naam
    //document.BEIDApplet.getFirstName1();
    //document.BEIDApplet.getSex();
    //document.BEIDApplet.getStreet();
    //document.BEIDApplet.getStreetNumber();
    //document.BEIDApplet.getBoxNumber();
    //document.BEIDApplet.getZip();
    //document.BEIDApplet.getMunicipality();
    //document.BEIDApplet.getCountry();

   

}
function EmptyScreen()
{
    var strTemp = "";
    document.getElementById('inputName').value = strTemp;
    document.getElementById('txtVoornaam').value = strTemp;
//    document.getElementById('drpGeslacht').value = strTemp;
    document.getElementById('txtAdres').value = strTemp;
    document.getElementById('txtPostcode').value = strTemp;
    document.getElementById('txtGemeente').value = strTemp;
    document.getElementById('txtLand').value = strTemp;
}

function ReadCard()
{
  var retval;
      EmptyScreen();
      document.getElementById('StatusField').innerHTML = "Reading Card, please wait...";
      retval = document.BEIDApplet.InitLib(null);
      if(retval == 0)
      {
        document.getElementById('StatusField').innerHTML = "Reading Identity, please wait...";
        getIDData();
//        document.getElementById('StatusField').innerHTML = "Reading Picture, please wait...";
//        document.BEIDApplet.GetPicture();
	  document.BEIDApplet.ExitLib();
        document.getElementById('StatusField').innerHTML = "Done";
      }
	else
      {
        document.getElementById('StatusField').innerHTML = "Error Reading Card";
      }
}
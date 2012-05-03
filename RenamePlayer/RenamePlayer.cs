using Hooks;
using TShockAPI;
using Terraria;
using System;
using System.ComponentModel;
using System.Text;

// RenamePlayer ****************************************************************
namespace RenamePlayer
{
  // PlayerInfo ****************************************************************
  [APIVersion(1, 11)]
  public class RenamePlayer : TerrariaPlugin
  {
    private TSPlayer player;
    private string   newName;
    private bool     nameChanged = false;

    #region Plugin Overrides
    // Initialize ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public override void Initialize()
    {
      ServerHooks.Join     += OnJoin;
      NetHooks.GreetPlayer += OnGreetPlayer;
    } // Initialize ------------------------------------------------------------

    
    // Dispose +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    protected override void Dispose( bool disposing )
    {
      if ( disposing )
      {
         NetHooks.GreetPlayer -= OnGreetPlayer;
         ServerHooks.Connect  -= OnJoin;
         base.Dispose( disposing );
      } // if
    } // Dispose ---------------------------------------------------------------
    #endregion // Plugin Overrides _____________________________________________


    #region Plugin Hooks 
    // OnJoin ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    void OnJoin( int              playerId, 
                 HandledEventArgs eventArgs )
    {
      player = TShock.Players[playerId];
      byte[] byteArray  = Encoding.Unicode.GetBytes( player.Name );
      byte[] asciiArray = Encoding.Convert( Encoding.Unicode, Encoding.ASCII, byteArray );

      newName = Encoding.ASCII.GetString( asciiArray );

      if ( player.Name != newName )
      {
        
        //newName = newName.Replace( "?", Main.rand.Next( 10 ).ToString() ); // randomize numbers in case they are all ?
        newName = "guest_" + player.Name.ToString().GetHashCode().ToString( "x" ).Substring( 0, 4 );

        TShock.Utils.Broadcast( string.Format( "Player '{0}' has been renamed to '{1}'.", player.Name, newName ), 
                                Color.IndianRed );
        Log.Info( string.Format( "Player '{0}' has been renamed to '{1}'.", player.Name, newName ) );

        player.TPlayer.name = newName;
        nameChanged = true;
      } // if

    } // OnJoin ----------------------------------------------------------------

    
    // OnGreetPlayer +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    void OnGreetPlayer( int              playerId, 
                        HandledEventArgs eventArgs )
    {
      if ( nameChanged )
      {
        player.SendMessage( "Warning: Your name had invalid characters, it has been changed to: " + newName, 
                             Color.OrangeRed );
      } // if

    } // OnGreetPlayer ---------------------------------------------------------
    #endregion // Plugin Hooks _________________________________________________

    
    #region Plugin Properties
    // Name ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public override string Name
    {
      get { return "RenamePlayer"; }
    } // Name ------------------------------------------------------------------


    // Author ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public override string Author
    {
      get { return "Scavenger, _Jon"; }
    } // Author ----------------------------------------------------------------


    // Description +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public override string Description
    {
      get { return "Renames players with Invalid Characters"; }
    } // Description -----------------------------------------------------------


    // Version +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
   public override Version Version
    {
      get { return new Version( 1, 0, 3, 0 ); }
    } // Versin ----------------------------------------------------------------

    
    // RenamePlayer ************************************************************
    public RenamePlayer( Main game ) : base( game )
    {
      Order = -11; // must be first
    } // RenamePlayer ----------------------------------------------------------
    #endregion // Plugin Properties ____________________________________________


  } // RenamePlayer ============================================================

} // RenamePlayer ==============================================================

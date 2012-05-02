using Hooks;
using TShockAPI;
using Terraria;
using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Text;

// RenamePlayer ****************************************************************
namespace RenamePlayer
{
  // PlayerInfo ****************************************************************
  [APIVersion(1, 11)]
  public class RenamePlayer : TerrariaPlugin
  {

    #region Plugin Overrides
    // Initialize ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public override void Initialize()
    {
      NetHooks.GreetPlayer += OnGreetPlayer;    
    } // Initialize ------------------------------------------------------------

    
    // Dispose +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    protected override void Dispose( bool disposing )
    {
      if ( disposing )
      {
         NetHooks.GreetPlayer -= OnGreetPlayer;
         base.Dispose( disposing );
      } // if
    } // Dispose ---------------------------------------------------------------
    #endregion // Plugin Overrides _____________________________________________


    #region Plugin Hooks 
    // OnGreetPlayer +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    void OnGreetPlayer( int              playerId, 
                        HandledEventArgs eventArgs )
    {
      TSPlayer player  = TShock.Players[playerId];
      char[]   split   = player.Name.ToCharArray();
      string   newName = player.Name;
      
      byte[] byteArray   = Encoding.Unicode.GetBytes( player.Name );
      byte[] asciiArray  = Encoding.Convert( Encoding.Unicode, Encoding.ASCII, byteArray );
      newName = Encoding.ASCII.GetString( asciiArray );

      if ( player.Name != newName )
      {
        // randomize numbers in case they are all ?
        newName = newName.Replace( "?", Main.rand.Next( 10 ).ToString() );

        TShock.Utils.Broadcast( string.Format( "Player '{0}' has been renamed to '{1}.", player.Name, newName ), 
                                Color.IndianRed );
        Log.Info( string.Format( "Player '{0}' has been renamed to '{1}'.", player.Name, newName ) );

        player.TPlayer.name = newName;
        player.SendMessage( "Warning: Your name had invalid characters, it has been changed to: " + newName, 
                             Color.IndianRed );
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
      get { return new Version( 1, 0, 1, 0 ); }
    } // Versin ----------------------------------------------------------------

    
    // RenamePlayer ************************************************************
    public RenamePlayer( Main game ) : base( game )
    {
      Order = -11; // must be first
    } // RenamePlayer ----------------------------------------------------------
    #endregion // Plugin Properties ____________________________________________


  } // RenamePlayer ============================================================

} // RenamePlayer ==============================================================

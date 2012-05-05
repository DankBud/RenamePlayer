using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Threading;
using Hooks;
using Terraria;
using TShockAPI;
using System.Text.RegularExpressions;

// RenamePlayer ****************************************************************
namespace RenamePlayer
{
  // PlayerInfo ****************************************************************
  [APIVersion(1, 11)]
  public class RenamePlayer : TerrariaPlugin
  {
    private TSPlayer _player;
    private string   _newName;
    private bool     _nameChanged = false;
    private static string  configFile = Path.Combine( TShock.SavePath, "RenamePlayer.cfg"  );
    public  static Config  _config    = new Config();
    public static bool     _replaceWords  = false;


    #region Plugin Overrides
    // Initialize ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public override void Initialize()
    {
      ServerHooks.Join     += OnJoin;
      NetHooks.GreetPlayer += OnGreetPlayer;

      if ( _config.LoadConfig( configFile ) )
      {
        try {
          _replaceWords = bool.Parse( _config["enablewordreplace"] );
        } // try
        catch( KeyNotFoundException )
        {
          Console.WriteLine( "! Unable to parse 'enableWordReplace' from config file: " + configFile );
          Thread.Sleep( 5000 );
        } // catch
      } // if
      else {
        Console.WriteLine( "! Config failed to load, skipping name replacement." );
        Thread.Sleep( 5000 );
      } // else

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
      try
      {
      _player = TShock.Players[playerId];
      ConvertEncoding();
      ReplaceWords();
      
      if ( _player.Name != _newName )
      {
        TShock.Utils.Broadcast( string.Format( "Player '{0}' has been renamed to '{1}'.", _player.Name, _newName ), 
                                Color.IndianRed );
        Log.Info( string.Format( "Player '{0}' has been renamed to '{1}'.", _player.Name, _newName ) );

        _player.TPlayer.name = _newName;
        _nameChanged = true;
      } // if
      } // try 
      catch ( Exception exception ) 
      {
        Log.Error( string.Format( "! Error: {0}: {1}", exception.Message, exception.StackTrace ) );
      } // catch

    } // OnJoin ----------------------------------------------------------------

    
    // OnGreetPlayer +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    void OnGreetPlayer( int              playerId, 
                        HandledEventArgs eventArgs )
    {
      if ( _nameChanged )
      {
        _player.SendMessage( "Warning: Your name had invalid characters, it has been changed to: " + _newName, 
                             Color.OrangeRed );
      } // if

    } // OnGreetPlayer ---------------------------------------------------------
    #endregion // Plugin Hooks _________________________________________________

    
    #region Data Handling
    // ConvertEncoding +++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    private void ConvertEncoding()
    {
      byte[] byteArray  = Encoding.Unicode.GetBytes( _player.Name );
      byte[] asciiArray = Encoding.Convert( Encoding.Unicode, Encoding.ASCII, byteArray );

      _newName = Encoding.ASCII.GetString( asciiArray );
      if ( _player.Name != _newName )
      {
        _newName = "guest_" + _player.Name.ToString().GetHashCode().ToString( "x" ).Substring( 0, 4 );
      } // if

    } // ConvertEncoding -------------------------------------------------------

    
    // ReplaceWords ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    private void ReplaceWords()
    {

      if ( _replaceWords )
      {
        foreach( var entry in _config ) 
        {
          if ( _newName.ToLower().Contains( entry.Key.ToLower() ) ) {
            _newName = Regex.Replace( _newName, entry.Key, entry.Value, RegexOptions.IgnoreCase);
//            _newName = _newName.ToLower().Replace( entry.Key.ToLower(), entry.Value );
          } // if
        } // for
      } // if

    } // ReplaceWords ----------------------------------------------------------
    #endregion // Data Handling ________________________________________________

    
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

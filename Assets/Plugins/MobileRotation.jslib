mergeInto(LibraryManager.library, {
  RequestLandscapeFullscreen: function () {
    var gameContainer = document.querySelector("#unity-container");
    if (gameContainer && gameContainer.requestFullscreen) {
      gameContainer.requestFullscreen().then(function() {
        if (screen.orientation && screen.orientation.lock) {
          screen.orientation.lock("landscape").catch(function(err) {
            console.log("Orientation lock failed: ", err);
          });
        }
      });
    }
  }
});
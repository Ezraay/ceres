export function notifyUserOfTryingToReconnect() {
    // Get the overlay element
    var overlay = document.getElementById('overlay');
    if (!overlay) {
        overlay = document.createElement('div');
        overlay.id = 'overlay';
        document.body.appendChild(overlay);
    }

    // Set the message and display the overlay
    overlay.style.display = 'block';
    overlay.innerHTML = '<div class="spinner"></div><div>Trying to reconnect to server...</div>';

    // Grey out the page
    var body = document.getElementsByTagName('body')[0];
    body.style.opacity = '0.5';
}



export function hideOverlay() {
    var overlay = document.getElementById('overlay');
    if (overlay) {
      overlay.parentNode.removeChild(overlay);
    }
  
    var body = document.getElementsByTagName('body')[0];
    body.style.opacity = '1';
  }
  

  export function notifyUserOfGameEnd(gameResult) {
    // console.log(gameResult + " overlay");
    // Get the overlay element
    var overlay = document.getElementById('overlay');
    if (overlay) {
        hideOverlay();
    }
    overlay = document.createElement("div");
  overlay.id = "overlay";

  // Create wrapper div for message and button
  var wrapper = document.createElement("div");
  wrapper.className = "button-wrapper";

  // Create message div
  var message = document.createElement("div");
  message.className =
    gameResult == "win" ? "win-overlay-msg" : "loss-overlay-msg";
  message.innerHTML = gameResult == "win" ? "YOU WON" : "YOU LOST";

  // Create button
  var button = document.createElement("button");
  button.innerHTML = "Back to Lobby";
  button.onclick = function () {
    window.location.href = "/Lobby";
  };

  // Append message and button to wrapper
  overlay.appendChild(message);
  wrapper.appendChild(button);

  // Append wrapper to overlay
  overlay.appendChild(wrapper);
  document.body.appendChild(overlay);

    // Grey out the page
    var body = document.getElementsByTagName('body')[0];
    body.style.opacity = '0.5';
}
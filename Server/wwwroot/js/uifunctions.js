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
  
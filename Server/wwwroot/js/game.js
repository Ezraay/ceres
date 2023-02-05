"use strict";

var GameHubConnection = new signalR.HubConnectionBuilder().withUrl("/GameHub").build();



function fulfilled() {
    let userId = sessionStorage.getItem("userId");
    let gameId = sessionStorage.getItem("gameId");
    if (gameId == null){                            // joining as a spectator - getting gameid from the url
        const queryString = window.location.search;
        const urlParams = new URLSearchParams(queryString);
        gameId =  urlParams.get('gameid');
    }
    
    
    GameHubConnection.invoke("JoinGame", gameId, userId).then((res) => {
        console.log(res)
        
    });
    
}

function rejected() {

}


GameHubConnection.on("UpdatePlayersName", (p1Name, p2Name) => {
    console.log("UpdatePlayersName")
    document.getElementById("player1Name").innerText = p1Name;
    document.getElementById("player2Name").innerText = p2Name;
    // document.getElementById("P1Action").disabled = false;
})

// GameHubConnection.on("Player2Joined", (p2Name) => {
//     console.log("Player2Joined")
//     document.getElementById("P2Action").disabled = false;
// })

GameHubConnection.start().then(fulfilled, rejected)
.catch(function (err) {
    return console.error(err.toString());
});
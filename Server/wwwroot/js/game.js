import * as ui from "./uifunctions.js";

"use strict";


var GameHubConnection = new signalR.HubConnectionBuilder()
    // .withHubProtocol(new signalR.JsonHubProtocol({
    //     transferFormat: signalR.TransferFormat.Text,
    //     typeNameHandling: 2  // Equivalent to TypeNameHandling.All
    // })
    .withUrl("/GameHub")
    .build();

let userId = sessionStorage.getItem("userId");
let gameId = sessionStorage.getItem("gameId");

var ImPlayer1 = false; 
var ImPlayer2 = false; 


function fulfilled() {
    if (gameId == null){                            // joining as a spectator - getting gameid from the url
        const queryString = window.location.search;
        const urlParams = new URLSearchParams(queryString);
        gameId =  urlParams.get('gameid');
    }
    
    
    GameHubConnection.send("JoinGame", gameId, userId).catch(function (err) {
        return console.error(err.toString());
    });
    
}

function rejected() {
    
}

GameHubConnection.on("JoinedGame", msg =>{
    let res = msg.gameJoiningResult;
    console.log(res)
    if (res === "JoinedAsPlayer1") {
        ImPlayer1 = true;
        document.getElementById("P2Action").hidden = true;
        document.getElementById("P1Action").disabled =false;
        document.getElementById("player1Name").innerText += " (You)"
    }
    if (res === "JoinedAsPlayer2") {
        ImPlayer2 = true;
        document.getElementById("P1Action").hidden = true;
        document.getElementById("P2Action").disabled =false;
        document.getElementById("player2Name").innerText += " (You)"
    }
})

GameHubConnection.on("UpdatePlayersName", msg => {
    let p1Name = msg.player1Name;
    let p2Name = msg.player2Name;
    console.log("UpdatePlayersName")
    document.getElementById("player1Name").innerText = p1Name;
    document.getElementById("player2Name").innerText = p2Name;
    if (ImPlayer1){
        document.getElementById("player1Name").innerText += " (You)"
    }
    if (ImPlayer2){
        document.getElementById("player2Name").innerText += " (You)"
    }
})

GameHubConnection.on("ServerAction", actionMsg => {
    let action = actionMsg.action;
    console.log(action);
     
    // let actionObj = Object.assign(Object.prototype, action);
    var li = document.createElement("li");
    li.classList.add("nav-item");
    var aTab = document.createElement("a");
    aTab.classList.add("nav-link","active");
    li.appendChild(aTab);

    if (action.$type == "Ceres.Core.BattleSystem.OpponentDrawCardAction, Core"){
        // console.log("opponent's card");
        aTab.innerText = "card";
        if (ImPlayer1){
            var cardsul = document.getElementById("P2Cards")
        }
        if (ImPlayer2){
            var cardsul = document.getElementById("P1Cards")
        }
    }
    
    if (action.$type == "Ceres.Core.BattleSystem.DrawCardAction, Core"){
        // console.log("my card");
        aTab.innerText = action.card.data.name;
        if (ImPlayer1){
            var cardsul = document.getElementById("P1Cards")
        }
        if (ImPlayer2){
            var cardsul = document.getElementById("P2Cards")
        }
    }



    // aTab.href = "#"

    // removing previously active aTabs
    var previousActiveTab = cardsul.querySelector(".nav-link.active");
    if(previousActiveTab) {
        previousActiveTab.classList.remove("active");
    }

    cardsul.appendChild(li);



    // // append the tab pane to the tab content div
    // var tabContentDiv = document.createElement("div");
    // tabContentDiv.classList.add("tab-content");
    // tabContentDiv.appendChild(tabPane);

});


GameHubConnection.on("GameEnded", msg => {
    let reason = msg.reason;
    console.log("Game's ended with " + reason);
    if (ImPlayer1 && ["Player2Left", "Player1Win"].includes(reason)){
        ui.notifyUserOfGameEnd("win");
    }
    if (ImPlayer1 && ["Player2Win"].includes(reason)){
        ui.notifyUserOfGameEnd("loss");
    }
    if (ImPlayer2 && ["Player1Left", "Player2Win"].includes(reason)){
        ui.notifyUserOfGameEnd("win");
    }
    if (ImPlayer2 && ["Player1Win"].includes(reason)){
        ui.notifyUserOfGameEnd("loss");
    }
})

GameHubConnection.start().then(fulfilled, rejected)
.catch(function (err) {
    return console.error(err.toString());
});

GameHubConnection.onreconnecting(error => {
    console.assert(GameHubConnection.state === signalR.HubConnectionState.Reconnecting);
    ui.notifyUserOfTryingToReconnect(); // Your function to notify user.
});

GameHubConnection.onreconnected(() => {
    ui.hideOverlay();
});


// // Test the function by stopping and restarting the connection after 5 seconds
// setTimeout(function () {
//     ui.notifyUserOfTryingToReconnect();
//     setTimeout(function () {
//         ui.hideOverlay();
//     }, 5000);
// }, 5000);


document.getElementById("P1Action").addEventListener("click", PlayerCommand);
document.getElementById("P2Action").addEventListener("click", PlayerCommand);

function PlayerCommand(){
    console.log("sending command");
    var playerCommand = {$type: "Ceres.Core.BattleSystem.TestDrawCommand, Core"};
    GameHubConnection.send("PlayerSentCommand", gameId, userId, playerCommand).catch(function (err) {
        return console.error(err.toString());
    });
    // document.getElementById("P1Action").disabled = !Player1Turn;
    // document.getElementById("P2Action").disabled = Player1Turn;

}


"use strict";


var GameHubConnection = new signalR.HubConnectionBuilder()
    // .withHubProtocol(new signalR.JsonHubProtocol({
    //     transferFormat: signalR.TransferFormat.Text,
    //     typeNameHandling: 2  // Equivalent to TypeNameHandling.All
    // }))
    .withUrl("/GameHub").build();
var Player1Turn = true;

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
    
    
    GameHubConnection.invoke("JoinGame", gameId, userId).then((res) => {
        console.log(res)
        if (res == "JoinedAsPlayer1") {
            ImPlayer1 = true;
            document.getElementById("P2Action").hidden = false;
            document.getElementById("P1Action").disabled =false;
            document.getElementById("player1Name").innerText += " (You)"
        }
        if (res == "JoinedAsPlayer2") {
            ImPlayer2 = true;
            document.getElementById("P1Action").hidden = false;
            document.getElementById("P2Action").disabled =false;
            document.getElementById("player2Name").innerText += " (You)"
        }
        
    });
    
}

function rejected() {
    
}


GameHubConnection.on("UpdatePlayersName", (p1Name, p2Name) => {
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

GameHubConnection.on("ServerAction", action => {
    console.log(action);
     
    let actionObj = Object.assign(Object.prototype, action);

    console.log(actionObj.card.data.name);



    var li = document.createElement("li");
    li.classList.add("nav-item");
    var aTab = document.createElement("a");
    aTab.classList.add("nav-link","active");
    aTab.innerText = actionObj.card.data.name;
    aTab.href = "#"
    li.appendChild(aTab);
    if (ImPlayer1){
        var cardsul = document.getElementById("P1Cards")
    }
    if (ImPlayer2){
        var cardsul = document.getElementById("P2Cards")
    }

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


GameHubConnection.start().then(fulfilled, rejected)
.catch(function (err) {
    return console.error(err.toString());
});




document.getElementById("P1Action").addEventListener("click", PlayerCommand);
document.getElementById("P2Action").addEventListener("click", PlayerCommand);

function PlayerCommand(){
    console.log("sending command");
    var playerCommand = new Object();
    GameHubConnection.send("PlayerSentCommand", gameId, userId, playerCommand).catch(function (err) {
        return console.error(err.toString());
    });
    // document.getElementById("P1Action").disabled = !Player1Turn;
    // document.getElementById("P2Action").disabled = Player1Turn;

}
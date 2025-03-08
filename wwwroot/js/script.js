const connection = new signalR.HubConnectionBuilder()
    .withUrl("/ticTacToeHub")
    .build();

connection.start().catch(err => console.error(err.toString()));

let board = ["", "", "", "", "", "", "", "", ""];
let currentPlayer = "X";
let gameMode = "AI"; // Default mode: AI
let xWins = 0, oWins = 0, draws = 0;
let gameOver = false; // Prevents multiple updates

// Handle player move
function makeMove(index) {
    if (board[index] !== "" || gameOver) return; // Prevent extra moves after game ends

    board[index] = currentPlayer;
    updateBoard();

    if (gameMode === "Multiplayer") {
        connection.invoke("MakeMove", currentPlayer, index % 3, Math.floor(index / 3))
            .catch(err => console.error(err.toString()));
    }

    let winner = checkWinner();
    if (winner) {
        updateScoreboard(winner);
        highlightWinningLine();
        gameOver = true; // Prevents multiple draw increments
        return;
    }

    currentPlayer = currentPlayer === "X" ? "O" : "X";

    if (gameMode === "AI" && currentPlayer === "O") {
        setTimeout(aiMove, 500);
    }
}

// Handle multiplayer move
connection.on("ReceiveMove", (player, row, col) => {
    let index = row * 3 + col;
    board[index] = player;
    updateBoard();

    let winner = checkWinner();
    if (winner) {
        updateScoreboard(winner);
        highlightWinningLine();
        gameOver = true;
    } else {
        currentPlayer = player === "X" ? "O" : "X";
    }
});

// AI Move (Simple Random AI)
function aiMove() {
    let availableMoves = board.map((val, idx) => val === "" ? idx : null).filter(val => val !== null);
    if (availableMoves.length === 0 || gameOver) return; // Prevent AI from playing after game ends

    let move = availableMoves[Math.floor(Math.random() * availableMoves.length)];
    board[move] = "O";
    updateBoard();

    let winner = checkWinner();
    if (winner) {
        updateScoreboard(winner);
        highlightWinningLine();
        gameOver = true;
    }

    currentPlayer = "X";
}

// Update board UI
function updateBoard() {
    document.querySelectorAll(".square").forEach((square, i) => {
        square.innerText = board[i];
        square.classList.remove("glow-x", "glow-o");

        if (board[i] === "X") square.classList.add("glow-x");
        if (board[i] === "O") square.classList.add("glow-o");
    });
}

// Check for a winner or a draw
function checkWinner() {
    const winPatterns = [
        [0, 1, 2], [3, 4, 5], [6, 7, 8], // Rows
        [0, 3, 6], [1, 4, 7], [2, 5, 8], // Columns
        [0, 4, 8], [2, 4, 6]            // Diagonals
    ];

    for (let pattern of winPatterns) {
        let [a, b, c] = pattern;
        if (board[a] && board[a] === board[b] && board[a] === board[c]) {
            return currentPlayer; // Return winner ("X" or "O")
        }
    }

    if (!board.includes("") && !gameOver) {
        return "Draw"; // Return "Draw" only once
    }

    return null;
}

// Highlight the winning line
function highlightWinningLine() {
    let winningPattern = checkWinner();
    if (winningPattern !== "Draw") {
        let winner = checkWinner();
        if (winner && winner !== "Draw") {
            let pattern = [
                [0, 1, 2], [3, 4, 5], [6, 7, 8], // Rows
                [0, 3, 6], [1, 4, 7], [2, 5, 8], // Columns
                [0, 4, 8], [2, 4, 6]            // Diagonals
            ].find(pattern =>
                board[pattern[0]] &&
                board[pattern[0]] === board[pattern[1]] &&
                board[pattern[0]] === board[pattern[2]]
            );

            if (pattern) {
                pattern.forEach(i => document.querySelectorAll(".square")[i].classList.add("win-line"));
            }
        }
    }
}

// Update scoreboard
function updateScoreboard(winner) {
    if (winner === "X") xWins++;
    else if (winner === "O") oWins++;
    else if (winner === "Draw" && !gameOver) {
        draws++;
    }

    document.getElementById("xWins").innerText = xWins;
    document.getElementById("oWins").innerText = oWins;
    document.getElementById("draws").innerText = draws;
}

// Reset game
document.getElementById("resetGame").addEventListener("click", () => {
    board.fill("");
    currentPlayer = "X";
    gameOver = false; // Reset game state
    updateBoard();

    document.querySelectorAll(".square").forEach(square => square.classList.remove("win-line"));

    if (gameMode === "Multiplayer") {
        connection.invoke("ResetGame").catch(err => console.error(err.toString()));
    }
});

// Reset scoreboard
document.getElementById("resetScoreboard").addEventListener("click", () => {
    xWins = 0;
    oWins = 0;
    draws = 0;
    document.getElementById("xWins").innerText = xWins;
    document.getElementById("oWins").innerText = oWins;
    document.getElementById("draws").innerText = draws;
});

// Handle game reset from SignalR
connection.on("GameReset", () => {
    board.fill("");
    gameOver = false;
    updateBoard();
});

// Toggle game mode
document.getElementById("aiMode").addEventListener("click", () => {
    gameMode = "AI";
    alert("AI Mode Activated!");
});

document.getElementById("multiMode").addEventListener("click", () => {
    gameMode = "Multiplayer";
    alert("Multiplayer Mode Activated!");
});

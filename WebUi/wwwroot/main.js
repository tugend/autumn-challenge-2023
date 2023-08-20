const main = async () => {
    /**
     * @typedef {object} Game
     * @property {number} turn
     * @property {number[][]} grid
     *
     * @param {Game} seed
     */
    const seed = {
        "turn": 1,
        "grid": [
            [1, 1, 0],
            [1, 0, 0],
            [0, 0, 0]
        ]
    };
    
    // TODO: enable paging (at 5 states left, try fetch next if more exists), 
    // TODO: enable pausing (stop fetching states, and stop play, click again -> continue)
    // TODO: enable seeding (prompt for new state given new seed, animate from there)
    // TODO: add reset button (clear all and toggle pause)

    /**
     * @param {Game[]} data
     */
    const data = await fetch("/api/conway", {
        method: "POST",
        headers: {'Content-Type': 'application/json'},
        body: JSON.stringify(seed)
    }).then(response => response.json());

    const turnElm = document.querySelector("#turn > span");
    const stateElm = document.querySelector("#state");

    /**
     * @param {number[][]} grid
     */
    const stringify = (grid) => grid
        .map((line, i) => line
            .map((value, j) => `<div onclick="onCellClick(${i}, ${j})">${value}</div>`)
            .join(""))
        .join("")

    const gameState = { foo: undefined, index: 0, update: null }
    window.gameState = gameState;
    function update()
    {
        let index = gameState.index++;
        if (index >= data.length) return;
        
        console.debug("Update", index);
        const state = data[index];
        turnElm.innerText = state.turn;
        stateElm.innerHTML = stringify(state.grid);
        window.gameState.foo = setTimeout(() => update(), 3000);
    }
    window.update = update;
    update();
};

function onCellClick(i, j) {
    console.log('TODO: handle cell click with coordinates', i, j)
}

function onTogglePause(i, j) {
    console.log('TODO: Toggle pause feature');
    
    const elm = document.getElementById('pause-btn');
    
    elm.innerText = elm.innerText.toLowerCase() === "pause" 
        ? "Continue" 
        : "Pause";
    
    // TODO: clean up the dirty state management with window...    
    if (window.gameState.foo !== null)
    {
        clearTimeout(window.gameState.foo);
        window.gameState.foo = null;
    }
    else {
        window.update()
    }
}

_ = main()
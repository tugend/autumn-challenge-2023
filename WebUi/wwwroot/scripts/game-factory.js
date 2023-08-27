window.conway = window.conway || {};

window.conway.gameFactory = (states, onStateChange) => {
    let timeoutId = null;
    let index = 0;

    const deepCopy = (instance) =>
        JSON.parse(JSON.stringify(instance));
    
    const isPaused = () =>
        timeoutId == null;

    const pause = () => {
        if (isPaused()) return;
        
        clearTimeout(timeoutId)
        timeoutId = null;
        onStateChange(current(), isPaused())
    }

    const nextTurn = () => {
        timeoutId = setTimeout(() => {
            index = Math.min(states.length -1, index + 1);
            onStateChange(states[index], isPaused());
            nextTurn();
        }, 2000);
    }

    const unpause = () => {
        timeoutId = setTimeout(nextTurn, 2000);
        onStateChange(current(), isPaused())
    }
    
    const current = () =>
        states[index];
    
    const seed = async (i, j) => {
        let seededState = deepCopy(states[0])
        seededState.grid[i][j] = seededState.grid[i][j] > 0 ? 0 : 1;
        seededState.turn = 0;
        return seededState;
    }
    
    const togglePause = () =>
        isPaused()
            ? unpause()
            : pause();
    
    const reset = () => {
        pause();
        index = 0;
        onStateChange(current(), isPaused())
    }
    
    const init = () => 
        onStateChange(current(), isPaused())

    return {
        init,
        pause,
        isPaused,
        unpause,
        current,
        seed,
        togglePause,
        reset
    };
} 
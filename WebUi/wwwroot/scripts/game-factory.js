window.conway = window.conway || {};

window.conway.gameFactory = (turnSpeedInMs, states) => {
    const turnInMs = turnSpeedInMs;
    let timeoutId = null;
    let index = 0;
    const initialStates = states;
    
    let onStateChange = () => {};

    const setState = (_states) => {
        states = _states.map(x => { x.turn = states[index].turn + x.turn; return x; });
        index = 0;
        triggerUpdate()
    }
    
    const deepCopy = (instance) =>
        JSON.parse(JSON.stringify(instance));
    
    const isPaused = () =>
        timeoutId == null;

    const pause = () => {
        if (isPaused()) return that;
        
        clearTimeout(timeoutId)
        timeoutId = null;
        onStateChange()
        
        return that;
    }

    const nextTurn = () => {
        timeoutId = setTimeout(() => {
            index = Math.min(states.length -1, index + 1);
            onStateChange();
            nextTurn();
        }, turnInMs);
    }

    const unpause = () => {
        timeoutId = setTimeout(nextTurn, turnInMs);
        onStateChange()
        return that;
    }
    
    const current = () =>
        states[index];
    
    const seed = async (i, j) => {
        let seededState = deepCopy(current()) // TODO: messy!
        seededState.grid = seededState.grid.map(xs => xs.map(x => x + ""));
        seededState.grid[i][j] = seededState.grid[i][j] > 0 ? "0" : "1";
        seededState.turn = 0;
        return seededState;
    }
    
    const togglePause = () =>
        isPaused()
            ? unpause()
            : pause();
    
    const reset = () => {
        states = initialStates;
        index = 0;
        onStateChange()
    }
    
    const triggerUpdate = () => {
        onStateChange()
        return that;
    }

    const that = {
        triggerUpdate,
        setState,
        pause,
        isPaused,
        unpause,
        current,
        seed,
        togglePause,
        reset,
        subscribe: { toChanged: (f) => onStateChange = () => f(current(), isPaused()) }
    };
    
    return that;
} 
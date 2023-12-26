window.conway = window.conway || {};

/**
 * @param { Number } turnSpeedInMs
 * @param { State[] } states
 * @returns { Game }
 */
window.conway.gameFactory = (turnSpeedInMs, states) => {
    const turnInMs = turnSpeedInMs;
    let timeoutId = null;
    let index = 0;
    const initialStates = states;
    
    let onStateChange = () => {};
    let onNextStatePage = () => {};
    
    const setState = (_states) => {
        states = _states.map(x => { x.turn = states[index].turn + x.turn; return x; });
        index = 0;
        triggerUpdate();
    };
    
    const deepCopy = (instance) =>
        JSON.parse(JSON.stringify(instance));
    
    const isPaused = () =>
        timeoutId == null;

    const pause = () => {
        if (isPaused()) return that;
        
        clearTimeout(timeoutId);
        timeoutId = null;
        onStateChange();
        
        return that;
    };

    const nextTurn = () => {
        console.log("next turn", index);

        if (index === states.length - 1) {
            console.log("next turn is at an end!");
            onNextStatePage(); // async
        }

        timeoutId = setTimeout(() => {
            index = Math.min(states.length -1, index + 1);
            onStateChange();
            nextTurn();
        }, turnInMs);
    };

    const unpause = () => {
        timeoutId = setTimeout(nextTurn, turnInMs);
        onStateChange();
        return that;
    };

    /**
     * @returns { State }
     */
    const current = () =>
        states[index];
    
    const seed = async (i, j) => {
        const seededState = deepCopy(current());
        
        const entry = seededState.grid[i][j];
        seededState.grid[i][j] = entry > 0 ? 0 : 1;
        seededState.turn = 0;
        
        return seededState;
    };
    
    const togglePause = () =>
        isPaused()
            ? unpause()
            : pause();
    
    const reset = () => {
        states = initialStates;
        index = 0;
        onStateChange();
    };
    
    const triggerUpdate = () => {
        onStateChange();
        return that;
    };

    /**
     * @type { Game }
     */
    const that = {
        triggerUpdate,
        setState,
        pause,
        unpause,
        togglePause,
        seed,
        reset,
        subscribe: {
            toChanged: (f) => onStateChange = () => f(current(), isPaused()),
            toNextStatePage: (f) => onNextStatePage = async () => {
                const nextPage = await f(states[index]); // TODO: this is ugly
                states = [...states.slice(0, states.length - 1), ...nextPage];
            }
        }
    };
    
    return that;
};
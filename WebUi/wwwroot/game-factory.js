window.conway = window.conway || {};

window.conway.gameFactory = (states, onStateChange) => {
        const that = {};

    let timeoutId = null;

    that.isPaused = () =>
        timeoutId == null;

    that.pause = () => {
        if (that.isPaused()) return;
        
        clearTimeout(timeoutId)
        timeoutId = null;
        onStateChange(that.current(), that.isPaused())
    }
    
    that.nextTurn = () => {
        timeoutId = setTimeout(() => {
            onStateChange(that.next(), that.isPaused());
            that.nextTurn();
        }, 2000);
    }

    that.unpause = () => {
        timeoutId = setTimeout(that.nextTurn, 2000);
        onStateChange(that.current(), that.isPaused())
    }

    let index = 0;
    that.index = () => index;
    that.incrementIndex = () => index++;
    that.resetIndex = () => index = 0;

    that.current = () =>
        states[index];

    that.next = () =>
    {
        console.log(index)
        return index + 1 < states.length
            ? states[++index]
            : states[index];
    }

    const deepCopy = (instance) => 
        JSON.parse(JSON.stringify(instance));
    
    that.seed = async (i, j) => {
        let seededState = deepCopy(states[0])
        seededState.grid[i][j] = seededState.grid[i][j] > 0 ? 0 : 1;
        seededState.turn = 0;
        return seededState;
    }
    
    that.togglePause = () =>
        that.isPaused()
            ? that.unpause()
            : that.pause();
    
    that.reset = () => {
        that.pause();
        index = 0;
        onStateChange(that.current(), that.isPaused())
    }

    return that;
} 
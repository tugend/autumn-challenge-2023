window.conway = window.conway || {};

window.conway.backendClient = (() => {
    const that = {};

    /**
     * @param { Response } response
     */
    const asJson = response => response.json()

    /**
     * @param { Object } content
     * @returns { RequestInit }
     */
    const postRequest = (content) => ({
        method: "POST",
        headers: {'Content-Type': 'application/json'},
        body: JSON.stringify(content)
    });

    /**
     * @typedef {object} State
     * @property {number} turn
     * @property {number[][]} grid
     *
     * @param {State} seed
     */
    
    /**
     * @param { State } seed
     * @return { Promise<State[]> }
     */
    that.fetchStates = (seed) =>
        fetch("/api/conway", postRequest(seed)).then(asJson);
    
    
    return that;
})();
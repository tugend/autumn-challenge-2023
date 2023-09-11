window.conway = window.conway || {};

/**
 * @param {Request | string | URL} fetchUrl
 */
window.conway.backendClientFactory = (fetchUrl) => {
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
    const fetchStates = (seed) =>
        fetch(fetchUrl, postRequest(seed)).then(asJson);
    
    return { fetchStates };
}
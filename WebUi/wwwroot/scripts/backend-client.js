window.conway = window.conway || {};

window.conway.BackendClient = class {
    /** @type {string} */
    #fetchUrl;
        
    /** @param {string} fetchUrl */
    constructor(fetchUrl) {
        this.#fetchUrl = fetchUrl;
    }

    /**
     * @param { State } seed
     * @return { Promise<State[]> }
     */
    fetchStates = (seed) => 
        fetch(this.#fetchUrl, this.#postRequest(seed))
            .then(this.#asJson);

    /**
     * @param { Response } response
     * @returns { Promise<string>}
     */
    #asJson = (response) => response.json()

    /**
     * @param { State } content
     * @returns { RequestInit } 
     */
    #postRequest = (content) => ({
        method: "POST",
        headers: {'Content-Type': 'application/json'},
        body: JSON.stringify(content)
    });    
}

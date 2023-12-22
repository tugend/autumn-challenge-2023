window.conway = window.conway || {};

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

    const getRequest = (content) => ({
        method: "GET",
        headers: {'Content-Type': 'application/json'},
        body: JSON.stringify(content)
    });

    /**
     * @typedef {object} State
     * @property {number} turn
     * @property {string[][]} grid
     *
     * @param {State} seed
     */
    
    /**
     * @param { State } seed
     * @return { Promise<State[]> }
     */
    const fetchStates = (seed) =>
        fetch(fetchUrl + "/states", postRequest(seed)).then(asJson);

    /**
     * @typedef {object} CatalogEntry
     * @property {string} key
     * @property {string[][]} value
     */
    
    /**
     * @return { Promise<CatalogEntry[]> }
     */
    const getCatalog = () =>
        fetch(fetchUrl + "/catalog", getRequest()).then(asJson);
    
    return { fetchStates, getCatalog };
}
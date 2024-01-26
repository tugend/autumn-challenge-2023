const postRequest = (content) => ({
    method: "POST",
    headers: {"Content-Type": "application/json"},
    body: JSON.stringify(content)
});

const getRequest = (content) => ({
    method: "GET",
    headers: {"Content-Type": "application/json"},
    body: JSON.stringify(content)
});

export default class EngineClient {
    /**
     * @type { string }
     */
    #baseUrl;

    /**
     * @param {string} baseUrl
     */
    constructor(baseUrl) {
        this.#baseUrl = baseUrl;
    }

    /**
     * @param { State } seed
     * @return { Promise<State[]> }
     */
    fetchStates = async (seed) => {
        const response = await fetch(this.#baseUrl + "/states", postRequest(seed));
        return response.json();
    }

    /**
     * @return { Promise<CatalogEntry[]> }
     */
     getCatalog = async () => {
        const response = await fetch(this.#baseUrl + "/catalog", getRequest());
        return response.json();
    }
}
﻿window.conway = window.conway || {};

window.conway.backendClientFactory = (fetchUrl) => {
    const asJson = response => response.json()

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
     * @param { State } seed
     * @return { Promise<State[]> }
     */
    const fetchStates = (seed) =>
        fetch(fetchUrl + "/states", postRequest(seed)).then(asJson);

    /**
     * @return { Promise<CatalogEntry[]> }
     */
    const getCatalog = () =>
        fetch(fetchUrl + "/catalog", getRequest()).then(asJson);
    
    return { fetchStates, getCatalog };
}
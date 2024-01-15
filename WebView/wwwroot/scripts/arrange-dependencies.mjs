import UrlClient from "./url-client.mjs";

/**
 * @param {DomClient} domClient
 * @param {UrlClient} urlClient
 * @param {GameController} controller
 * @param {GameClient} gameClient
 * @param {Settings} settings
 * @param {CatalogEntry[]} catalog
 */
const configure = (settings, urlClient, domClient, controller, gameClient, catalog) => {
    domClient.subscribe.toCellClick(async (i, j) => {
        controller.pause();
        const newSeed = await controller.seed(i, j);
        const newStates = await gameClient.fetchStates(newSeed);
        controller.setState(newStates);
    });

    domClient.subscribe.toResetBtnClick(() => {
        controller.pause();
        controller.reset();
    });

    domClient.subscribe.toTogglePlayBtnClick(controller
        .togglePause);

    domClient.subscribe.toCatalogSelect(catalogIndex => {
        urlClient.setSettings({
            turn: 0,
            color: domClient.getColor(),
            turnSpeedInMs: settings.turnSpeedInMs,
            optionalSeedOverride: catalog[catalogIndex]
        });
    });

    controller.subscribe.toChanged(domClient.rerender);
}

export default configure;

/// <reference types="cypress" />

import { BasePageObject } from "../page-objects/BasePageObject"



describe('Renders base page', () => {
    it('Checks if you render base page', async () => {
        let { search_bar } = await cy.fixture('main.json')

        // Dashboard
        let basePage = new BasePageObject()
        basePage.visit()
        basePage.elementByText(search_bar).should('exist')
    })
})

describe('A journey to visit', () => {
    it('allows the user to visit', async () => {
        let { search_bar } = await cy.fixture('main.json')
        // Dashboard
        let basePage = new BasePageObject()

        basePage.visit()

        cy.get('input[name="main-search-item"]').type('Eindhoven, Netherlands')

        basePage.elementByText("Search").click()

        basePage.elementByText(search_bar).should('exist')
    })
})
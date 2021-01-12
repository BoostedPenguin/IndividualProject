export class BasePageObject {
    baseUrl = 'http://localhost:3000/'

    visit() {
        cy.visit(this.baseUrl)
    }

    elementByText(query) {
        return cy.contains(query)
    }
}
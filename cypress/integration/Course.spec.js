describe('Student Test', () => {
    before(() => {
        // set up application so first stop is setup for test

    })
    beforeEach(() => {
        //navigate to bus tracker page
        cy.visit('https://faceattendance.azurewebsites.net/Courses')
    })
    after(() => {
        //clean up 

    })
    it('Add new Course', () => {
        //click on create student button
        cy.get('#CreateStudent').click()
        cy.get('#StudentCode').type(12345)
        cy.get('#StudentName').type("Steve")
        cy.get('#active').click()
        cy.get('#submit').click()
        cy.get('tr').last().find('#StudentCode').last().contains("12345")
        cy.get('tr').last().find('#StudentName').last().contains("Steve")
        cy.get('tr').last().find('#Active :checked').last().should('be.checked')

    })
    
    
})
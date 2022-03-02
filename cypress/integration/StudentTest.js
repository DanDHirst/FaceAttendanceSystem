describe('Student Test', () => {
    before(() => {
        // set up application so first stop is setup for test

    })
    beforeEach(() => {
        //navigate to bus tracker page
        cy.visit('https://faceattendance.azurewebsites.net/Students')
    })
    after(() => {
        //clean up 

    })
    it('Add new Student', () => {
        //click on create student button
        cy.get('#CreateStudent').click()
        cy.get('#StudentCode').type(12345)
        cy.get('#StudentName').type("Steve")
        cy.get('#active').click()
        cy.get('#submit').click()
        cy.get('tr').last().find('#StudentCode').last().should('have.text','12345')

    })
    
})
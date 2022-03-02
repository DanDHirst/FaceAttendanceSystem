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
        cy.get('tr').last().find('#StudentCode').last().contains("12345")
        cy.get('tr').last().find('#StudentName').last().contains("Steve")
        cy.get('tr').last().find('#Active :checked').last().should('be.checked')

    })
    it('Edit new Student', () => {
        //click on create student button
        cy.get('tr').last().find('#EditStudent').last().click()
        cy.get('#Student_StudentCode').clear().type(12346)
        cy.get('#Student_StudentName').clear().type("John")
        cy.get('#Student_active').click()
        cy.get('#Save').click()

        

    })
    
})
describe('Student Test', () => {
    before(() => {
        // set up application so first stop is setup for test

    })
    beforeEach(() => {
        //navigate to bus tracker page
        cy.visit('https://faceattendance.azurewebsites.net/Classes')
        cy.get('#Input_Email').type('admin@123.com')
        cy.get('#Input_Password').type('Password123!')
        cy.get('.btn').click()
        cy.visit('https://faceattendance.azurewebsites.net/Classes')
    })
    after(() => {
        //clean up 

    })
    it('Add new Class', () => {
        //click on create student button
        cy.get('p > .btn').click()
        cy.get('#StartDateTime').type("2017-06-01T08:30")
        cy.get('#EndDateTime').type("2017-06-01T08:30")
        cy.get(':nth-child(7) > .btn').click()

        

    })
    it('Edit new Student', () => {
        //click on edit student button
        cy.get('tr').last().find('#Edit').last().click()
        cy.get('#Room').select('SMB0')
        cy.get(':nth-child(8) > .btn').click()
        cy.get('tr').last().find('#room').last().contains("SMB0")
    })
    it('Delete Student', () => {
        //click on details student button
        cy.get('tr').last().find('#Delete').last().click()
        cy.get('#Delete').click()
    })
    
})
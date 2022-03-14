
describe('Module Test', () => {
    before(() => {
        // set up application so first stop is setup for test

    })
    beforeEach(() => {
        //navigate to bus tracker page
        
        cy.visit('https://faceattendance.azurewebsites.net/Modules')
        cy.get('#Input_Email').type('admin@123.com')
        cy.get('#Input_Password').type('Password123!')
        cy.get('.btn').click()
        cy.visit('https://faceattendance.azurewebsites.net/Modules')
    })
    after(() => {
        //clean up 

    })
    it('Add a new Module', () => {
        //click on create student button
        cy.get('#Create').click()
        cy.get('#ModuleCode').type("Comp 3008")
        cy.get('#ModuleName').type("Big Data")
        cy.get('#Create').click()
        cy.get('tr').last().find('#ModuleCode').last().contains("Comp 3008")
        cy.get('tr').last().find('#ModuleName').last().contains("Big Data")
    })
    it('Edit the new Module', () => {
        //click on create student button
        cy.get('tr').last().find('#Edit').click()
        cy.get('#ModuleCode').clear().type("Comp 3009")
        cy.get('#ModuleName').clear().type("Big Datas")
        cy.get('#Save').click()
        cy.get('tr').last().find('#ModuleCode').last().contains("Comp 3009")
        cy.get('tr').last().find('#ModuleName').last().contains("Big Datas")
    })

    it('View Details of new Module', () => {
        //click on create student button
        cy.get('tr').last().find('#Details').click()
        cy.get('#ModuleCode').contains("Comp 3009")
        cy.get('#ModuleName').contains("Big Datas")
        //check module is good
        //add a course and attach to Module
        cy.visit('https://faceattendance.azurewebsites.net/Courses')
        cy.get('#Create').click()
        cy.get('#CourseCode').type("3008")
        cy.get('#CourseName').type("Big Data")
        cy.get('#Create').click()
        //add module then delete
        cy.get('tr').last().find('#Details').click()
        cy.get('#EnterModuleCode').type("Comp 3009")
        cy.get('#AddModule').click()

        cy.visit('https://faceattendance.azurewebsites.net/Modules')
        cy.get('tr').last().find('#Details').click()
        cy.get('#CourseCode').contains("3008")
        cy.get('#CourseName').contains("Big Data")



        //remove course and module
        cy.visit('https://faceattendance.azurewebsites.net/Courses')
        cy.get('tr').last().find('#Details').click()
        cy.get('#DeleteModule').click()
        cy.visit('https://faceattendance.azurewebsites.net/Courses')
        cy.get('tr').last().find('#Delete').click()
        cy.get('#Delete').click()



    })



    it('Delete the new Module', () => {
        //click on create student button
        cy.get('tr').last().find('#Delete').click()
        cy.get('#ModuleCode').contains("Comp 3009")
        cy.get('#ModuleName').contains("Big Datas")
        cy.get('#Delete').click()
    })
    
    
})
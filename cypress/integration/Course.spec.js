

describe('Courses Test', () => {
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
        cy.get('#Create').click()
        cy.get('#CourseCode').type("3008")
        cy.get('#CourseName').type("Big Data")
        cy.get('#Create').click()
        cy.get('tr').last().find('#CourseCode').last().contains("3008")
        cy.get('tr').last().find('#CourseName').last().contains("Big Data")
    })
    it('edit new Course', () => {
        //click on create student button
        cy.get('tr').last().find('#Edit').click()
        cy.get('#CourseCode').clear().type("3009")
        cy.get('#CourseName').clear().type("Operating systems")
        cy.get('#Submit').click()
        cy.get('tr').last().find('#CourseCode').last().contains("3009")
        cy.get('tr').last().find('#CourseName').last().contains("Operating systems")
    })
    it('Course Details', () => {
        //click on create student button
        cy.get('tr').last().find('#Details').click()
        cy.get('.row > :nth-child(2)').contains("3009")
        cy.get('.row > :nth-child(4)').contains("Operating systems")
        //add module then delete
        cy.get('#EnterModuleCode').type("Comp3005")
        cy.get('#AddModule').click()
        cy.get('#ModuleName').contains("project managment")
        cy.get('#ModuleCode').contains("Comp3005")
        cy.get('#DeleteModule').click()

        //add student then delete
        cy.get('#EnterStudentID').type("12232")
        cy.get('#AddStudent').click()
        cy.get('#StudentCode').contains("12232")
        cy.get('#StudentName').contains("Alexander")
        cy.get('#DeleteStudent').click()


       

        

    })
    it('Delete new Course', () => {
        //click on create student button
        cy.get('tr').last().find('#Delete').click()
        cy.get('#CourseCode').contains("3009")
        cy.get('#CourseName').contains("Operating systems")
        cy.get('#Delete').click()
    })
    
    
    
})
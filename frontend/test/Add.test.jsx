import { fireEvent, getByText, render, screen } from '@testing-library/react'
import Add from '../src/pages/Add'
import UniversitiesProvider from '../src/UniversitiesProvider'
import { BrowserRouter, Routes } from 'react-router-dom'




describe('Add', () => {
    it('renders the Add component', () => {
        render(
            <UniversitiesProvider>
                <BrowserRouter>
                <Add />
                </BrowserRouter>
            </UniversitiesProvider>
        )

        fireEvent.click(screen.getByText("Add to list"))

        //expect(setUniList).toHaveBeenCalled();
    })
})



const mockFn = vi.fn();
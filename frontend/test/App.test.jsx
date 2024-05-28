import { fireEvent, render, screen } from '@testing-library/react'
import App from '../src/App'
import { vi } from 'vitest'


describe('App', () => {
  beforeAll(() => {
    Object.defineProperty(window, 'matchMedia', {
      writable: true,
      value: vi.fn().mockImplementation(query => ({
        matches: false,
        media: query,
        onchange: null,
        addListener: vi.fn(), // deprecated
        removeListener: vi.fn(), // deprecated
        addEventListener: vi.fn(),
        removeEventListener: vi.fn(),
        dispatchEvent: vi.fn(),
      })),
    });
  })
  
  it('renders the App component', () => {
    render(<App />)

    fireEvent.click(screen.getByText("Add university"));
    
    fireEvent.change(
        screen.getByTestId("nameAdd").getElementsByTagName("input")[0], 
        {target: {value: "NYU"}});
    fireEvent.click(screen.getByText("Add to list"));
    fireEvent.click(screen.getByTestId("KeyboardArrowRightIcon"));
    
    expect(screen.getByText("NYU")).toBeTruthy();

    fireEvent.click(screen.getAllByText("Edit")[0]);
    expect(screen.getByTestId("nameEdit").getElementsByTagName("input")[0].value).toBe("New York University");
    fireEvent.change(
        screen.getByTestId("nameEdit").getElementsByTagName("input")[0], 
        {target: {value: "NYUUU"}});
        
    fireEvent.click(screen.getByText("Update element"));
    fireEvent.click(screen.getByTestId("KeyboardArrowRightIcon"));
    expect(screen.getByText("NYUUU")).toBeTruthy();

    fireEvent.click(screen.getByTestId("id106"));
    fireEvent.click(screen.getByTestId("del106"));
    expect(screen.queryByText("NYUUU")).toBeNull();
  })
})

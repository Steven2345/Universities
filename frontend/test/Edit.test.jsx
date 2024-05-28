import { render, screen } from '@testing-library/react'
import Edit from '../src/pages/Edit'
import UniversitiesProvider from '../src/UniversitiesProvider'
import { MemoryRouter, Routes, Route } from 'react-router-dom';
import { vi } from 'vitest';

/*const mockUniv = [
    createUni("Massachusetts Institute of Technology", "Cambridge, Massachusetts", 100, 
      "The Massachusetts Institute of Technology (MIT) is a private land-grant research university in Cambridge, Massachusetts. \
      Established in 1861, MIT has played a significant role in the development of many areas of modern technology and science."),
  createUni("Harvard University", "Cambridge, Massachusetts", 98.3, 
      "Harvard University is a private Ivy League research university in Cambridge, Massachusetts. Founded in 1636 as Harvard \
      College and named for its first benefactor, the Puritan clergyman John Harvard, it is the oldest institution of higher \
      learning in the United States."),
  createUni("University of California, Berkeley", "Berkeley, California", 90.4, 
      "The University of California, Berkeley is a public land-grant research university in Berkeley, California. Founded in \
      1868 and named after Anglo-Irish philosopher George Berkeley, it is the state's first land-grant university and the \
      founding campus of the University of California system."),
]*/

test('Edit', () => {
    
        const { getByText } = render(
            <UniversitiesProvider>
                <Edit />
            </UniversitiesProvider>
        )
        const nameInput = getByText('Name')//.getElementsByTagName('input')[0];s
})

vi.mock('react-router-dom', async () => {
    const mod = await vi.importActual<typeof import('react-router-dom')>('react-router-dom');
    return {
        ...mod, 
        useNavigate: () => mockFn,
        useParams: () => ({ id: '102' }),
        Link: () => mockFn
    };
});

const mockFn = vi.fn();
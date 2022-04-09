import { MeuCarroTemplatePage } from './app.po';

describe('MeuCarro App', function() {
  let page: MeuCarroTemplatePage;

  beforeEach(() => {
    page = new MeuCarroTemplatePage();
  });

  it('should display message saying app works', () => {
    page.navigateTo();
    expect(page.getParagraphText()).toEqual('app works!');
  });
});

function getData() {
  return [
    {
      'id': 'item_1',      'name': '条',       'rowHeight': 50,
      'actual': { 
          'label': {
          'value': '第一条，某某某内容000',
          'fontColor': '#FFF',
          'fontSize': 14,
          'position': anychart.enums.Position.LEFT_CENTER,
          //'anchor': anychart.enums.Anchor.RIGHT_CENTER
        },
           'fill': {    'keys': ['#283848']  }
      }      
    }, 
       {
      'id': 11,
      'name': '项',
      'parent': 'item_1',     
      'rowHeight': 50,
      'actual': { 
           'label': {
                'value': '第一项，某某某内容',
                'fontColor': '#FFF',
                'fontSize': 14,
                'position': anychart.enums.Position.LEFT_CENTER
                },
           'fill': {    'keys': ['#525252']  }
          }  
      },       
       {
      'id': 112,
      'name': '细项',
      'parent': 11,     
      'rowHeight': 50,
      'actual': { 
            'label': {
                'value': '细项001，某某某内容',
                'fontColor': '#FFF',
                'fontSize': 14,
                'position': anychart.enums.Position.LEFT_CENTER        
              },
           'fill': {    'keys': ['#10467E']  }
          }  
      },
      {
        'id': 1121,
        'name': '文件',
        'parent': 112,
        'actualStart': Date.UTC(2014, 1, 10),
        'actualEnd': Date.UTC(2014, 1, 18),
        'rowHeight': 30,
        'actual': { 
              'label': {
              'value': '某某某文件，某某某内容',
              'fontColor': '#FFF',
              'fontSize': 14,
              'position': anychart.enums.Position.LEFT_CENTER        
              },
             'fill': {    'keys': ['#4288D0']  }
            }  
      },
       {
        'id': 1122,
        'name': '文件',
        'parent': 112,
        'actualStart': Date.UTC(2014, 1, 10),
        'actualEnd': Date.UTC(2014, 1, 19),
        'rowHeight': 30,
        'actual': { 
              'label': {
              'value': '某某某文件，某某某内容',
              'fontColor': '#FFF',
              'fontSize': 14,
              'position': anychart.enums.Position.LEFT_CENTER        
              },
             'fill': {    'keys': ['#4288D0']  }
            }  
      }, 
       {
      'id': 113,
      'name': '细项',
      'parent': 11,     
      'rowHeight': 50,
      'actual': { 
            'label': {
                'value': '细项002，某某某内容',
                'fontColor': '#FFF',
                'fontSize': 14,
                'position': anychart.enums.Position.LEFT_CENTER        
              },
           'fill': {    'keys': ['#10467E']  }
          }  
      },
       {
        'id': 1131,
        'name': '文件',
        'parent': 113,
        'actualStart': Date.UTC(2014, 1, 10),
        'actualEnd': Date.UTC(2014, 1, 18),
        'rowHeight': 30,
        'actual': { 
              'label': {
              'value': '某某某文件，某某某内容',
              'fontColor': '#FFF',
              'fontSize': 14,
              'position': anychart.enums.Position.LEFT_CENTER        
              },
             'fill': {    'keys': ['#4288D0']  }
            }  
      },
       {
        'id': 1132,
        'name': '文件',
        'parent': 113,
        'actualStart': Date.UTC(2014, 1, 12),
        'actualEnd': Date.UTC(2014, 1, 17),
        'rowHeight': 30,
        'actual': { 
              'label': {
              'value': '某某某文件，某某某内容',
              'fontColor': '#FFF',
              'fontSize': 14,
              'position': anychart.enums.Position.LEFT_CENTER        
              },
             'fill': {    'keys': ['#4288D0']  }
            }  
      },
  ];
}